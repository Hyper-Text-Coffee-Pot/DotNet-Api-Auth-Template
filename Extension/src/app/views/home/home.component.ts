import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { BasePageDirective } from '../shared/base-page.directive';

@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BasePageDirective
{
	constructor(private route: ActivatedRoute, private titleService: Title)
	{
		super(route, titleService);
	}

	public override ngOnInit(): void
	{
		// Add your code here
	}
}
