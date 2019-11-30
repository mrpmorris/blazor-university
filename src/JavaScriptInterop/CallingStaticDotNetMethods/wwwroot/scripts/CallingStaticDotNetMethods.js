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

// Use Blazor.start - Don't forget to set autostart="false" in /Pages/_Host.cshtml
//Blazor.start({})
//	.then(async function () {
//		const settings = await DotNet.invokeMethodAsync("CallingStaticDotNetMethods", "GetSettings");
//		alert('API key: ' + settings.someApiKey);
//	});
