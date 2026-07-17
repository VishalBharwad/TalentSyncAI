# TalentSync AI — Build Log

## Step 1 (this delivery): Domain + Infrastructure foundation

**What's included:**
- `TalentSyncAI.Domain` — all entities (ApplicationUser, Company, Recruiter, Candidate, Job,
  Skill/JobSkill/CandidateSkill, Resume, Application, InterviewSchedule, Notification, AuditLog),
  enums, and repository/unit-of-work interfaces.
- `TalentSyncAI.Infrastructure` — `ApplicationDbContext` (Identity + domain tables with full
  Fluent API configuration: relationships, cascade rules, unique indexes), generic repository +
  Unit of Work implementation, and `DbSeeder` (creates Admin/Recruiter/Candidate roles + a default
  admin account).

**How to assemble it on your machine (you'll need the .NET 8 SDK installed):**

```bash
# from the TalentSyncAI/ folder
dotnet new sln -n TalentSyncAI
dotnet sln add src/TalentSyncAI.Domain/TalentSyncAI.Domain.csproj
dotnet sln add src/TalentSyncAI.Infrastructure/TalentSyncAI.Infrastructure.csproj
dotnet sln add src/TalentSyncAI.Application/TalentSyncAI.Application.csproj
# (Web project .csproj arrives in Step 2, along with Program.cs, Identity wiring, and the first migration)
dotnet build
```

## Step 2 (this delivery): Web project, Identity, Auth, Areas

**What's included:**
- `TalentSyncAI.Web` — full MVC project: `Program.cs` (EF Core + Identity + Serilog + cookie
  config + area routing + startup role/admin seeding), `appsettings.json`, Bootstrap 5 layout,
  responsive nav with role-aware login partial.
- **Auth**: `AccountController` with Register (Candidate/Recruiter self-select), Login (with
  lockout + remember-me), Logout, AccessDenied, and centralized post-login role redirect.
  On registration, the matching `Candidate`/`Recruiter` profile row is created in the same
  transaction as the Identity user.
- **Areas**: `Admin`, `Recruiter`, `Candidate` — each with a `[Authorize(Roles = ...)]`-protected
  `DashboardController` and placeholder view, proving role-based routing/authorization end-to-end.
- `TalentSyncAI.sln` ties all four projects together.

**Default seeded admin login** (created automatically on first run): `admin@talentsync.ai` /
`Admin@12345` — **change this immediately** in any real deployment.

**How to get this running (requires .NET 8 SDK + SQL Server or LocalDB):**

```bash
cd TalentSyncAI
dotnet restore
dotnet ef migrations add InitialCreate --project src/TalentSyncAI.Infrastructure --startup-project src/TalentSyncAI.Web
dotnet ef database update --project src/TalentSyncAI.Infrastructure --startup-project src/TalentSyncAI.Web
dotnet run --project src/TalentSyncAI.Web
```

If `dotnet ef` isn't installed: `dotnet tool install --global dotnet-ef`.

Adjust `ConnectionStrings:DefaultConnection` in `appsettings.json` if you're not using LocalDB
(e.g. a full SQL Server instance or a Docker container). For real credentials (SMTP password,
Gemini/OpenAI API key), don't put them in `appsettings.json` — use
`dotnet user-secrets init && dotnet user-secrets set "AiSettings:GeminiApiKey" "..."` from the
Web project folder, or environment variables in production.

**Try it:** run the app, register as a Candidate and separately as a Recruiter, and confirm each
lands on their own dashboard. Try visiting `/Admin/Dashboard` while logged in as a Candidate —
you should be redirected to Access Denied, confirming role-based authorization is working.

## Step 3 (this delivery): Core CRUD + Apply Flow

**What's included:**
- **Recruiter side**: `CompanyController` (one company profile per recruiter, pending admin
  approval), `JobController` (full CRUD; delete is a soft-close, not a hard delete, to preserve
  application history), `ApplicationsController` (`ByJob` view of applicants sorted by match
  score once AI is wired in, with an ownership check on status updates so a recruiter can't touch
  another company's applications).
- **Candidate side**: `ProfileController` (bio/skills/experience), `ResumeController` (upload
  with server-side PDF/DOCX + 5MB validation, GUID filenames on disk, primary-resume logic),
  `JobsController` (search/filter open jobs + apply flow with resume selection and a cover note),
  `ApplicationsController` (status tracking).
- **Services layer** (`TalentSyncAI.Application.Services`): `CompanyService`, `JobService`,
  `CandidateProfileService`, `ResumeService`, `ApplicationService` — all registered in `Program.cs`.
  A shared `SkillParser` turns comma-separated skill input into normalized, deduplicated `Skill`
  rows for both jobs and candidate profiles.
- Applying automatically creates a `Notification` for the recruiter; status changes automatically
  notify the candidate (the `Notifications` table is now actively used, not just modeled).
- Dashboards now link to all of the above instead of showing placeholder cards.

**Design decisions worth calling out:**
- Services query `ApplicationDbContext` directly for reads that need `Include`/filtering/paging
  (search, applicant lists), rather than forcing everything through the generic repository —
  the repository/UoW from Step 1 is still used for simple writes elsewhere, but wrapping every
  `Include`-heavy read behind `IGenericRepository<T>` would mean re-inventing LINQ through leaky
  abstractions. This is deliberate, not an inconsistency.
- Job deletion is a soft-close (`Status = Closed`) rather than a real `DELETE`, since a hard
  delete would cascade-remove `Applications` a candidate already submitted.
- Resume files are stored on disk under `wwwroot/uploads/resumes/{candidateId}/{guid}.ext` with
  the original filename kept only for display — never trust a user-supplied filename for storage.

**Try it:** as a Recruiter, create a company profile, post an "Open" job. As a Candidate, upload a
resume, fill out your profile, search for the job you just posted, and apply. Switch back to the
Recruiter and open "Manage Jobs → applicant count" to see and update the application status —
then check the Candidate's "My Applications" page to see the status reflect the change.

## Coming in Step 4
- AI Engine (`IAiService` + Gemini/OpenAI implementation): resume parsing into `Resume.ParsedJson`
  / `AiSummary`, skill extraction, `Application.MatchScore` + `MatchExplanation` computation,
  candidate ranking (already sorted by `MatchScore` in the applicant view — just needs real
  numbers), job recommendations for candidates, and interview question generation.
- Interview scheduling (recruiter picks a date/time per application, MailKit sends the invite).
- Admin module: user/company approval & management, reports, audit log viewer.

## Design decisions made so far
- **4-project layering** (Domain/Infrastructure/Application/Web) so business logic and the AI
  provider can be swapped without touching MVC controllers.
- **Repository + Unit of Work**: justified here because several flows (submitting an application
  updates Application + Notification + AuditLog together) need one atomic `SaveChanges()`.
- **ApplicationUser extends IdentityUser**, with `Candidate`/`Recruiter` as 1-1 side tables keyed
  by `UserId` — avoids duplicating auth fields while keeping role-specific data separate and clean.
- **Resume.ParsedJson**: AI-extracted structured resume data is stored as JSON rather than
  exploded into many tables, since it's read-heavy display data, not something we run relational
  queries against directly (skills *are* separately normalized into `CandidateSkill` for querying/matching).
- **Unique index on (CandidateId, JobId)** in Applications enforces "one application per job per
  candidate" at the database level, not just in application code.
