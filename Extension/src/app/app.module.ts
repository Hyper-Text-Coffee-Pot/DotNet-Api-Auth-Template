import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './views/not-found/not-found.component';
import { HomeComponent } from './views/home/home.component';
import { BasePageDirective } from './views/shared/base-page.directive';
import { LoginComponent } from './views/identity/login/login.component';
import { provideFirebaseApp, initializeApp } from '@angular/fire/app';
import { getAuth, provideAuth } from '@angular/fire/auth';
import { environment } from 'src/environments/environment';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SignupComponent } from './views/identity/signup/signup.component';
import { ForgotPasswordComponent } from './views/identity/forgot-password/forgot-password.component';
import { ActionComponent } from './views/identity/action/action.component';
import { BlockUIModule } from 'ng-block-ui';
import { RECAPTCHA_V3_SITE_KEY, RecaptchaV3Module } from 'ng-recaptcha';
import { AuthService } from './domain/auth/services/auth.service';
import { AuthInterceptor } from './domain/auth/interceptors/auth.interceptor';

@NgModule({
	declarations: [
		AppComponent,
		NotFoundComponent,
		HomeComponent,
		BasePageDirective,
		LoginComponent,
		SignupComponent,
		ForgotPasswordComponent,
		ActionComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		FormsModule,
		ReactiveFormsModule,
		HttpClientModule,
		provideFirebaseApp(() => initializeApp(environment.firebaseConfig)),
		provideAuth(() => getAuth()),
		RecaptchaV3Module,
		BlockUIModule.forRoot()
	],
	providers: [
		AuthService,
		{
			provide: HTTP_INTERCEPTORS,
			useClass: AuthInterceptor,
			multi: true
		},
		{
			provide: RECAPTCHA_V3_SITE_KEY,
			useValue: environment.reCAPTCHASiteKey
		}
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
