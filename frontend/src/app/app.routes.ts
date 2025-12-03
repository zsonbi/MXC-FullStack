import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { EventListComponent } from './components/event-list/event-list.component';
import { EventFormComponent } from './components/event-form/event-form.component';
import { authGuard, guestGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    component: LoginComponent,
    //If logged in redirect
    canActivate: [guestGuard]
  },
  {
    path: 'events',
    component: EventListComponent,
    //Only logged in
    canActivate: [authGuard]
  },
  {
    path: 'events/new',
    component: EventFormComponent,
    //Only logged in
    canActivate: [authGuard]
  },
  {
    path: 'events/edit/:id',
    component: EventFormComponent,
    //Only logged in
    canActivate: [authGuard]
  },
  { path: '**', redirectTo: 'login' }
];