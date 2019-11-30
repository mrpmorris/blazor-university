setTimeout(async function () {
	let settings = await DotNet.invokeMethodAsync("CallingStaticDotNetMethods", "GetSettings");
	console.log(settings);
}, 1000);