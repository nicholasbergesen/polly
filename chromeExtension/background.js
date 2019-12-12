https://www.chromium.org/Home/chromium-security/extension-content-script-fetches
chrome.runtime.onMessage.addListener(
    function(request, sender, sendResponse) {
        console.log("backgroundcalled");
        if (request.contentScriptQuery == "fetchProduct") {
            var url = "https://api.takealot.com/rest/v-1-9-0/product-details/" + request.itemId + "?platform=desktop";
            fetch(url)
            .then(response => response.text())
            .then(text => sendResponse(text))
            return true;  // Will respond asynchronously.
        }
    }
);