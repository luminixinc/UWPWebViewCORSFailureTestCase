INSTRUCTIONS:

Place the HelloWorldMsAppData.html file in the Local AppData path for the app package. This location should be something like:

```
C:\Users\username\AppData\Local\Packages\<PackageIdOrGUID>\LocalState\
```

Build and run the application, and make sure you see the appropriate content in the app window (e.g. "Hello World"). Now Launch the Microsoft Edge DevTools Preview app (https://www.microsoft.com/en-us/p/microsoft-edge-devtools-preview/9mzbfrmz0mnj?activetab=pivot:overviewtab), and choose the "hello world" entry from the main window. You should be able to see the console, network tab, and debugger for the app.

When you press the "Get Data" button, you will see a message similar to the following: "TypeError: Failed to fetch". The console will show an error similar to: 

```
SEC7120: [CORS] The origin 'ms-local-stream://7df9b159-4022-44da-8a91-ae6edf516b7b_4d7957656256696577' did not find 'ms-local-stream://7df9b159-4022-44da-8a91-ae6edf516b7b_4d7957656256696577' in the Access-Control-Allow-Origin response header for cross-origin  resource at 'https://www.bing.com/'.
```

This happens because the ORIGIN header sent via the CORS Preflight request contains the local "ms-local-stream" url. 

![Screenshot](/images/Screenshot_1.png)

QUESTION: Does the WebView have a configuration that can modify the Origin header on the CORS Preflight request to comply with what the server is looking for?
