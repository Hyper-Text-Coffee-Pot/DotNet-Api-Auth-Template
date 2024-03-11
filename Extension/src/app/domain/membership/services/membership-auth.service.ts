import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, retry } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MemberAccountCreateRequest } from '../models/member-account-create-request';

@Injectable({
	providedIn: 'root'
})
export class MembershipAuthService
{
	constructor(private _httpClient: HttpClient) { }

	public CreateNewMemberAccount(memberAccountCreateRequest: MemberAccountCreateRequest): Observable<any>
	{
		return this._httpClient
			.post(`${ environment.apiUrlV1 }/membership-auth/signup-with-email-and-password`, memberAccountCreateRequest)
			.pipe(
				retry(3)
			);
	}

	public SignInWithGoogle(memberAccountCreateRequest: MemberAccountCreateRequest): Observable<any>
	{
		return this._httpClient
			.post(`${ environment.apiUrlV1 }/membership-auth/sign-in-with-google`, memberAccountCreateRequest)
			.pipe(
				retry(3)
			);
	}

	/**
	 * Update the last login for the current user.
	 * @param memberGuid 
	 * @returns a string
	 */
	public SignInWithEmailAndPassword(authToken: string, authProviderUID: string, reCAPTCHAToken: string): Observable<any>
	{
		return this._httpClient.post(`${ environment.apiUrlV1 }/membership-auth/sign-in-with-email-and-password?authToken=${ authToken }&authProviderUID=${ authProviderUID }&reCAPTCHAToken=${ reCAPTCHAToken }`, null,)
			.pipe(
				retry(3)
			);
	}
}
