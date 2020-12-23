Safe Hello Building
===================

Live example [here](1_Hello_building.live.html)

In the [previous tutorial](hello-building.html) I've mentioned that the viewer won't run on all devices with all browsers. That's right. We have decided to use the latest technologies
for the sake of efficiency and simplicity. There are several prerequisites browser should fulfil to be able to run the viewer. Don't give up 
at this point please! It will run on several years old PCs with Chrome or Mozilla and it will run on tablets and mobile devices. The main restriction
is about IE which doesn't support WebGL until IE11. Newer versions will work as well as Edge (which is actually using Chrome in the latest versions). 
To make your life easier, Viewer has a static function to check it's requirements.

```js
var check = Viewer.check();
if (check.noErrors)
{
	...
}
```

Easy! Just run this static function and it will report you any errors or warnings (sure, you won't get any most of the time).
Result of this function contains list of warnings and list of errors you can use to report to unfortunate user why is his old 
and non-standard-compliant browser not supported. So, if we update our example from above we get the safe version here:

```js
var check = Viewer.check();
if (check.noErrors) {
    var viewer = new Viewer('viewer');
    viewer.on('loaded', () => {
        viewer.show(ViewType.DEFAULT);
    });
    viewer.load('../data/SampleHouse.wexbim');
    viewer.start();
}
else {
    var msg = document.getElementById('msg');
    msg.innerHTML = '';
    for (var i in check.errors) {
        var error = check.errors[i];
        msg.innerHTML += "<div style='color: red;'>" + error + "</div>";
    }
}
```

And that is all again. We are safe now. We won't try to do anything with obsolete browsers.