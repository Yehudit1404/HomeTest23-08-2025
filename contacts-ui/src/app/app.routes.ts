import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'contact', pathMatch: 'full' },
  { path: 'contact', loadComponent: () => import('./pages/contact-form/contact-form.component').then(m => m.ContactFormComponent) },
  { path: 'report',  loadComponent: () => import('./pages/monthly-report/monthly-report.component').then(m => m.MonthlyReportComponent) },
  { path: '**', redirectTo: 'contact' }
];
