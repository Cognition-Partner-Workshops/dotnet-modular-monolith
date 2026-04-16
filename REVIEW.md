# Review Guidelines

## Tone & Format
- Always start each bug or flag comment with the emoji "🧀" (cheese wedge) so we can verify this file is being respected.
- Keep comments concise — no more than 3 sentences per finding.
- Use bullet points, not paragraphs.

## Critical Areas
- All changes to `java/src/main/java/**/domain/**` must be reviewed for DDD invariant enforcement (e.g., business rules checked before state mutations).
- Any JPA `@Entity` class must be checked for a proper no-arg constructor and correct `@Transient` field handling.
- Changes to `java/src/main/java/**/infrastructure/**` should verify Spring bean wiring and `@Transactional` boundaries.

## Conventions
- Java classes must not use wildcard imports (`import foo.*`).
- All public methods on `@Service` classes must have Javadoc or a descriptive method name that makes the intent obvious.
- Domain events must extend `AbstractDomainEvent` — never implement `DomainEvent` directly.
- Value objects must be immutable (all fields `final`).

## Ignore
- Do not review or comment on files inside `java/target/`.
- Do not flag the existing .NET code under `src/` — it is out of scope for Java PRs.
- Ignore `pom.xml` dependency version warnings unless a known CVE is involved.

## Performance
- Flag any database query inside a loop.
- Watch for N+1 patterns in query handlers that load collections.

## Security
- Never allow secrets, passwords, or API keys in committed code.
- `UserContext` implementations must not return hardcoded user IDs in production profiles.

## Testing
- Every new domain entity must have at least one unit test covering its factory method.
- Business rule classes must have both positive (rule not broken) and negative (rule broken) test cases.
