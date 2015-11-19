# KoWatIn
I decided to upgrade awesome automation framework WatIn to work with IE not only as standalone application but as WebBrowser control embedded in some application.  With this upgrade developers can implement automations that target web applications. 
An example how it can be used is here:             
  var embeddedBrowser = new EmbededIE("ApplicationProcessName");            
  var htmlElement = embeddedBrowser.Element(Find.ById("element-id"));            
  var elementClass = htmlElement.ClassName;
