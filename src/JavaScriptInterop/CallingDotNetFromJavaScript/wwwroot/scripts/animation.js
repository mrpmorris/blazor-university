var BlazorUniversity = BlazorUniversity || {};
BlazorUniversity.animate = function (htmlElement, animationName, duration, dotNetObject) {
    let animationStartCallback = () => {
        dotNetObject.invokeMethodAsync('AnimationStarted', animationName);
    }

    let animationEndCallback = () => {
        // Code for Chrome, Safari and Opera
        htmlElement.removeEventListener('webkitAnimationStart', animationStartCallback);
        htmlElement.removeEventListener('webkitAnimationEnd', animationEndCallback);

        // Standard syntax
        htmlElement.removeEventListener('animationstart', animationStartCallback);
        htmlElement.removeEventListener('animationend', animationEndCallback);

        dotNetObject.invokeMethodAsync('AnimationEnded', animationName);
    }

    // Code for Chrome, Safari and Opera
    htmlElement.addEventListener('webkitAnimationStart', animationStartCallback);
    htmlElement.addEventListener('webkitAnimationEnd', animationEndCallback);

    // Standard syntax
    htmlElement.addEventListener('animationstart', animationStartCallback);
    htmlElement.addEventListener('animationend', animationEndCallback);

    let animText = `${animationName} ${duration}s`;
    htmlElement.style.WebkitAnimation = animText;
    htmlElement.style.animation = animText;
};
