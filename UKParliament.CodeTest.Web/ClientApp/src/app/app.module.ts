import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { PersonListComponent } from './components/person-list/person-list.component';

@NgModule({ declarations: [
        AppComponent,
        HomeComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: PersonListComponent, pathMatch: 'full' },
            { path: 'home', component: HomeComponent, pathMatch: 'full' }
        ])], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
