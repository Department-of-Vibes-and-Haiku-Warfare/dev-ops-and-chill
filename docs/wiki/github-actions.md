# Dev, Ops, and chill: GitHub Actions Workflows

This is the special sauce, where the automagik happens.

# dev-ops-and-chill.yml

Every morning at 2:17am we run a workflow that scrapes Donald's latest tweet, uses OpenAI API to analyze it for mood and vibe, then updates our main site index.html file and performs a signed commit.

Toolchain:

- A [Node.js project](https://github.com/Department-of-Vibes-and-Haiku-Warfare/dev-ops-and-chill/tree/main/src/scraper) that uses [puppeteer](https://www.npmjs.com/package/puppeteer) w/ [stealth plugin](https://www.npmjs.com/package/puppeteer-extra-plugin-stealth) to scrape the [latest tweet](https://x.com/realDonaldTrump) into a text file
- A [dotnet project](https://github.com/Department-of-Vibes-and-Haiku-Warfare/dev-ops-and-chill/tree/main/src/HaikuGenerator) that uses [OpenAI API](https://openai.com/api/) to analyze the text for mood and vibe, generates a subtly mocking haiku, and updates the index.html file.
- [GPGTools](https://gpgtools.org/) to [sign the commit](https://docs.github.com/en/authentication/managing-commit-signature-verification/signing-commits). Key and Key ID are [repo secrets](https://github.com/Department-of-Vibes-and-Haiku-Warfare/dev-ops-and-chill/settings/secrets/actions). [No passphrase](devops-postmortem-2025-04-02.md).

[Full workflow](https://github.com/Department-of-Vibes-and-Haiku-Warfare/dev-ops-and-chill/blob/main/.github/workflows/dev-ops-and-chill.yml)

# pr.yml

Builds the toolchain for PRs. Boilerplate. Nothing to see here.

# rubberstamp.yml [DEPRECATED]

An out-of-date workflow that automagikally approved and merged PRs from @ChaosTestOps based on some hashtags and other nonsense.

Deprecated in favor on [VibeStamp](https://github.com/Department-of-Vibes-and-Haiku-Warfare/vibe-stamp-bot) app.
