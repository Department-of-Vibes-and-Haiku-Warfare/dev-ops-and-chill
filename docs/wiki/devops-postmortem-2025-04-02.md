# DevOps Postmortem – April 2, 2025

## 🧨 What Broke

While trying to automatically generate and commit updates to `docs/index.html` using a .NET console app running in GitHub Actions, the workflow failed at the `git push` step with:

```
remote: Permission to Department-of-Vibes-and-Haiku-Warfare/dev-ops-and-chill.git denied to github-actions[bot].
fatal: unable to access 'https://github.com/...': The requested URL returned error: 403
```

## 🕵️ Root Causes

- **Used a fine-grained Personal Access Token (PAT)** with `contents: write` permission instead of a classic PAT.
- Fine-grained PATs don't always work properly with `git push` from GitHub Actions due to limitations in how GitHub authenticates fine-grained tokens during workflow execution.
- Even with the correct permissions set and access granted to all DOVAHW repos, Git still attempted to authenticate as `github-actions[bot]`, resulting in denied access.

## Root Causes

- Additional confusion was caused by the fact that I hadn't had any coffee and my sleep-deprived brain thought Git was reporting file insertions and deletions, not line insertions and deletions.

## 🔥 Fix

- Replaced the fine-grained PAT with a **classic PAT** with the `repo` scope.
- Updated the workflow to use this token explicitly via:
  ```yaml
  - uses: actions/checkout@v2
    with:
      token: ${{ secrets.BOT_PAT }}
  ```
- Ensured only `docs/index.html` was staged using:
  ```bash
  git add docs/index.html
  ```

## ✅ Lessons Learned

- Classic PATs are still more reliable than fine-grained ones in CI/CD pipelines — use them for now unless you **really** need fine-grained scope.
- Always validate what’s actually staged using `git diff --cached`.
- GitHub Actions will default to `github-actions[bot]` unless you override **both** the token and the remote URL early and correctly.
- Permission errors can cascade misleadingly — always validate the identity Git is using to push.

## 🍺 Takeaway

**This was a blood sacrifice to the CI gods, and it has been accepted.**

---

## 🧠 What We Learned About GPG Signing in CI

- `git commit -S` is not CI-friendly out of the box — it assumes an interactive TTY unless the GPG key is both **trusted** and **unprotected**.
- Passphrase-protected GPG keys are nearly impossible to use in GitHub Actions unless you use an insecure `expect` script, which still often fails due to `/dev/tty` limitations.
- Even when a GPG key has no passphrase, `git` will fail unless GPG fully trusts the key. The normal interactive `--edit-key trust` dialog can't run in CI.
- Use `gpg --import-ownertrust` with the full fingerprint and `6` (ultimate trust) to non-interactively trust the key:
  ```bash
  echo "$KEY_ID:6:" | gpg --import-ownertrust
  ```
- `gpg.program="gpg --batch"` does **not** work because Git interprets the string as a path. Instead:
  - Set `batch` mode in `~/.gnupg/gpg.conf`
  - Use `git -c gpg.program=gpg ...` in your workflow
- GitHub runners create a fresh GPG trustdb on every run — you must set trust every time.

## 🛠 Final Working Workflow Tips

- Use a no-passphrase GPG key stored in secrets as a base64 string.
- Decode and import the key at the start of your workflow.
- Use this to trust the key:
  ```bash
  KEY_ID=$(gpg --list-secret-keys --with-colons | grep '^fpr' | head -n1 | cut -d: -f10)
  echo "$KEY_ID:6:" | gpg --import-ownertrust
  ```
- Then commit like this:
  ```bash
  git -c gpg.program=gpg \
      -c commit.gpgsign=true \
      -c user.signingkey=$KEY_ID \
      commit -S -m "Signed haiku update"
  ```

## 🎯 Takeaway

**GPG signing in GitHub Actions is technically possible, but only if you're willing to lose your entire evening and your faith in humanity.**
