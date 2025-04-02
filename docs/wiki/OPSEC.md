# 🛡️ Department of Vibes and Haiku Warfare — OPSEC Guidelines

> Everybody knows you never go full retard.

-- [Kirk Lazarus](https://www.imdb.com/title/tt0942385/characters/nm0000375)

> ... except with operational security.

-- @ChaosTestOps

## 🔒 Digital Hygiene

- Use **ProtonVPN** at all times for encrypted traffic and IP masking.
- Manage all credentials and sensitive data with **ProtonPass**.
  - Generate strong, unique passwords for every account.
  - Use **ProtonPass aliases** for email signups per service (avoid reuse).
  - Store OTPs and passkeys directly in ProtonPass where supported.
- Always enable **2FA** (TOTP preferred) for every service.
- Use the **Tor Browser** for high-anonymity browsing — no bookmarks, no logins, and no extensions.

## 🧑‍💻 DevSecOps Practices

- **Sign every Git commit** using your GPG or SSH signing key.
- Enforce **branch protection rules**:
  - Require signed commits
  - Require PRs for all changes (except for trusted CI/CD bots)
  - Require reviews before merge
- Secure CI/CD secrets:
  - Use GitHub Actions secrets for all credentials (e.g. `OPENAI_API_KEY`)
  - Never hardcode secrets into code or workflow files

## 🔐 Communication Security

- Use **Signal** for all sensitive group chats and 1:1 comms.
  - Do NOT invite the President.
  - Do NOT invite journalists.
  - Assume every message is being archived.
- Destroy unused Signal groups regularly.

## 🖥️ Device Security

- Set up **separate OS user accounts** or profiles for operational identities (e.g. ChaosTestOps vs personal).
  - macOS: create a separate Admin account per identity.
  - Android: use multi-user profiles and/or work profile sandboxing (e.g. Shelter or Island).
- Regularly update all OS, browser, and device firmware.
- Encrypt your disks.

## 🏠 Physical Security

- Lock your front door.
- Use window locks and privacy film.
- Don’t brag about OPSEC on public social media.
- Don’t reuse vibe credentials on your Ring doorbell account.

## 🧠 Final Rule

> Treat every system like it’s compromised.  
> Treat every teammate like they might get raided.  
> Trust vibes. Verify everything else.
