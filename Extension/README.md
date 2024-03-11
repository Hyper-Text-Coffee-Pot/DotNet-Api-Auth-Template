# Extension

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 13.3.3.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build & Deploy

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.
Run `ng build dev` while testing to run the dev server.
See the `package.json` file for all possible build options.

This app uses [cheeriojs](https://cheerio.js.org/) to run a post-build step that overwrites the default
behavior of Angular for the compiled css. The default `media` attribute is `print` and for the extension
to render correctly and apply our custom css file we need it to be set to `all` right away. This behavior
is not a feature in Angular so we've added a cheeriojs post-build script that simply overwrites this
setting to be `all` instead. Pretty simple, but important to call out that this modification to the
compiled project has been automated.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.

## Important Additional Package Information
### Working with Browser Extensions
- https://developer.chrome.com/docs/extensions

### Firebase
- https://firebase.google.com/
- https://github.com/angular/angularfire
- https://firebaseopensource.com/projects/angular/angularfire2/

### Ng Block UI
- https://www.npmjs.com/package/ng-block-ui

### Ng Recaptcha
Get an account key from google, then add the package. You should set up a new reCAPTHCHA key for each environment.
Go to this Google link and then visit the v3 Admin Console to add projects. Copy your public site key in to the
reCAPTCHASiteKey value in the environment config file.
- https://www.google.com/recaptcha
- https://www.npmjs.com/package/ng-recaptcha

### Cheerio JS
Read above about Build & Deploy steps for information on how this is used. There is a `postbuild.js` file at the
root of the `Extension` project that contains more information and the configuration for this process.
- https://cheerio.js.org/