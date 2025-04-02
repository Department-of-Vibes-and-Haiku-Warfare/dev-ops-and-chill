# 📝 Postmortem: Twitter Scraping Woes (2025-04-02)

## 🧵 Problem

We needed to extract Trump's latest tweet text every time the GitHub Actions workflow ran. Initially, we assumed this would be a simple scrape using a headless browser or public proxy.

## 🔥 What Went Wrong

- ❌ **Nitter stopped working**: Requests returned an HTML blob with a Cloudflare JS challenge.
- ❌ **Unauthenticated scraping**: Twitter blocks headless Chromium by default, serving a "This browser is no longer supported" splash screen.
- ❌ **Playwright timeouts**: Even with user-agent overrides and headless tweaks, `waitForSelector()` on the tweet div failed due to aggressive detection.
- ❌ **Browser verification & fingerprinting**: Even navigating to the page failed due to bot detection tech (e.g. `navigator.webdriver`, lack of GPU, etc).

## 🛠️ Attempted Fixes

- ⏸ Switched to Playwright with full GUI (headful mode) — worked locally but not viable in CI.
- 🤖 Tried launching Chromium with `--no-sandbox` and `--disable-setuid-sandbox` — bypassed one layer of detection.
- 💣 Attempted login automation — blocked by JS challenges and timeout at the username field.
- 💭 Explored Nitter mirrors — all defunct or redirecting to spam.
- 💀 Considered Twitter API — requires $100/mo subscription.
- 😤 Failed attempt to reuse session via cookies — detected due to IP mismatch and headless fingerprint.

## ✅ Final Solution

We built a standalone Node.js app using:

- `puppeteer-extra` and `puppeteer-extra-plugin-stealth`
- Stealth-mode Chromium launched in CI with `--no-sandbox`
- Output saved to `scraper/latest-tweet.txt` for consumption by the haiku generator

This modular separation allowed us to run the Node scraper independently from the C# haiku pipeline while avoiding all authentication, rate limiting, and detection headaches.

## 🤯 Lessons Learned

- Don't try to fight Twitter's anti-bot system in headless mode.
- Puppeteer with stealth plugin is currently the most reliable workaround.
- Always decouple scraping logic from primary app logic.
- CI needs headless scraping tools to be tuned for realism (GPU flags, sandboxing, font support).
- It's not "just a scrape" anymore — platforms fight back.

## 😎 Current Vibe

> "We broke the internet to write poems." — The Department of Vibes and Haiku Warfare
