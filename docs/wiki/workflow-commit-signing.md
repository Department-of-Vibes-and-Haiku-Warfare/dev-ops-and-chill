# GitHub Actions Commit Signing with GPG – Setup Guide

This guide documents how to generate a GPG key, upload it to GitHub, and configure your GitHub Actions workflow to create signed commits.

---

## 🔐 Step 1: Generate a GPG Key

Run the following in a terminal under your bot account (e.g., ChaosTestBot):

```bash
gpg --full-generate-key
```

**Use the following values:**

- Key type: `RSA and RSA`
- Key size: `4096`
- Expiration: `0` (never expire)
- Name: `ChaosTestBot`
- Email: `205850019+ChaosTestBot@users.noreply.github.com`
- Comment: `CI Signing Key`
- Passphrase: optional (but remember it if used)

---

## 🆔 Step 2: Get the Key ID

```bash
gpg --list-secret-keys --keyid-format=long
```

Look for output like:

```
sec   rsa4096/ABCDEF1234567890 2025-04-01 [SC]
```

Copy the part after the slash (`ABCDEF1234567890`).

---

## 📤 Step 3: Export the Keys

### Export the **public key**:

```bash
gpg --armor --export ABCDEF1234567890 > chaosbot-public.asc
```

### Export the **private key**:

```bash
gpg --armor --export-secret-keys ABCDEF1234567890 > chaosbot-private.asc
```

Then encode the private key to base64:

```bash
base64 chaosbot-private.asc > chaosbot-private.asc.b64
```

---

## 🌐 Step 4: Upload Public Key to GitHub

While logged in as the ChaosTestBot account:

1. Go to https://github.com/settings/keys
2. Click "New GPG Key"
3. Paste the contents of `chaosbot-public.asc`
4. Save

---

## 🔐 Step 5: Add Secrets to GitHub Repo

In your target repository:

- Go to **Settings → Secrets and variables → Actions**
- Add the following secrets:

| Secret Name          | Value                                     |
| -------------------- | ----------------------------------------- |
| `BOT_GPG_KEY`        | Contents of `chaosbot-private.asc.b64`    |
| `BOT_GPG_KEY_ID`     | The GPG key ID (e.g., `ABCDEF1234567890`) |
| `BOT_GPG_PASSPHRASE` | (optional) Only if used during keygen     |

---

## 🛠️ Step 6: Update GitHub Actions Workflow

Add the following steps before your commit step:

```yaml
- name: Import and trust GPG key
  run: |
    echo "${{ secrets.BOT_GPG_KEY }}" | base64 --decode > chaosbot.asc
    gpg --batch --import chaosbot.asc

    echo "pinentry-mode loopback" >> ~/.gnupg/gpg.conf
    echo "use-agent" >> ~/.gnupg/gpg.conf

    KEY_ID=${{ secrets.BOT_GPG_KEY_ID }}
    echo -e "5\\ny\\n" | gpg --command-fd 0 --expert --edit-key $KEY_ID trust

    git config --global user.name "ChaosTestBot"
    git config --global user.email "205850019+ChaosTestBot@users.noreply.github.com"
    git config --global commit.gpgsign true
    git config --global user.signingkey $KEY_ID
    git config --global gpg.program gpg
```

---

## ✅ Step 7: Sign the Commit in the Workflow

If you set a passphrase, insert the following lines **immediately before the `git commit -S` line** in your GitHub Actions workflow step:

```bash
export GPG_TTY=$(tty)
echo "${{ secrets.BOT_GPG_PASSPHRASE }}" | \
  gpg --batch --yes --passphrase-fd 0 --pinentry-mode loopback
```

This ensures that the GPG key can be unlocked non-interactively during the commit step.

```bash
git commit -S -m "Signed haiku update"
```

---

That's it! Your commits from GitHub Actions will now be signed with a trusted GPG key and display the ✅ Verified badge in GitHub.
