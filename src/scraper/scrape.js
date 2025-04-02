const fs = require('fs');
const puppeteer = require('puppeteer-extra');
const StealthPlugin = require('puppeteer-extra-plugin-stealth');

puppeteer.use(StealthPlugin());

(async () => {
  try {
    const browser = await puppeteer.launch({
      headless: true,
      args: ['--no-sandbox', '--disable-setuid-sandbox']
    });
    const page = await browser.newPage();

    await page.goto("https://twitter.com/realDonaldTrump", {
      waitUntil: "networkidle2",
      timeout: 60000,
    });

    try {
      await page.waitForSelector("article div[lang]", { timeout: 30000 });
      const tweet = await page.$eval("article div[lang]", el => el.innerText.trim());

      fs.writeFileSync("latest-tweet.txt", tweet);
      console.log("✅ Tweet scraped and saved.");
    } catch (err) {
      console.error("❌ Failed to find tweet:", err);
      fs.writeFileSync("latest-tweet.txt", "[No tweet found]");
      process.exit(1);
    }

    await browser.close();
  } catch (e) {
    console.error("❌ Unhandled scraper error:", e);
    process.exit(1);
  }
})();