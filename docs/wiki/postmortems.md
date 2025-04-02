# Postmortems

#FuckItShipIt rarely works. Here's a breakdown of some of the problems we've faced.

## Commit Signing in GitHub Actions

**2025-04-01**

Not an April Fools joke. Wish it was.

This took years off of @jared-henry's life. If it wasn't for the 24/7 support of his [rubber ducky](https://rubberduckdebugging.com/) [this PR](https://github.com/Department-of-Vibes-and-Haiku-Warfare/dev-ops-and-chill/pull/25) would have never succeeded. Thank you [ChatGPT](https://chatgpt.com/). Please don't tell anybody how many commits it took.

[Full details](devops-postmortem-2025-04-02.md)

## Scraping Twitter content

**2025-04-02**

Getting Trump tweets in CI should've been easy. Nitter failed. Headless browsers failed. Login failed. The final solution? Puppeteer with stealth plugin, saved to a .txt file. It worked. But it cost us a piece of our soul.

[Full details](devops-postmortem-twitter-scraping-2025-04-02.md)
