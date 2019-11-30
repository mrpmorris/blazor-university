// Invoke Blazor after 1 second
setTimeout(async function () {
	const settings = await DotNet.invokeMethodAsync("CallingStaticDotNetMethods", "GetSettings");
	alert('API key: ' + settings.someApiKey);
}, 1000);

// Attempt to invoke Blazor every 10 milliseconds until successful
//window.someInitialization = async function () {
//	try {
//		const settings = await DotNet.invokeMethodAsync("CallingStaticDotNetMethods", "GetSettings");
//		alert('API key: ' + settings.someApiKey);
//	}
//	catch {
//		// Try again
//		this.setTimeout(someInitialization, 10);
//	}
//}
//window.someInitialization();
