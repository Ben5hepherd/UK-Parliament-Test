import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { PersonListComponent } from './components/person-list/person-list.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import Material from '@primeng/themes/material';
import { AddPersonComponent } from './components/add-person/add-person.component';
import { EditPersonComponent } from './components/edit-person/edit-person.component';

@NgModule({ declarations: [
        AppComponent,
        HomeComponent
    ],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: PersonListComponent, pathMatch: 'full' },
            { path: 'add', component: AddPersonComponent, pathMatch: 'full' },
            { path: 'edit/:id', component: EditPersonComponent, pathMatch: 'full' },
            { path: 'home', component: HomeComponent, pathMatch: 'full' }
        ])], providers: [
            provideHttpClient(withInterceptorsFromDi()),
            provideAnimationsAsync(),
            providePrimeNG({
                theme: {
                    preset: Material,
                    options: {
                        prefix: 'p',
                        darkModeSelector: 'light-mode',
                        cssLayer: false
                    }
                }
            })
        ] })
export class AppModule { }
