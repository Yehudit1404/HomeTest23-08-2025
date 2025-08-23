import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatToolbarModule, MatButtonModule, MatIconModule],
  template: `
    <mat-toolbar color="primary">
      <span class="brand">
        <mat-icon>assignment_ind</mat-icon>
        Contact Requests
      </span>
      <span class="spacer"></span>
      <a mat-button routerLink="/contact" routerLinkActive="active"><mat-icon>chat</mat-icon> טופס</a>
      <a mat-button routerLink="/report" routerLinkActive="active"><mat-icon>insights</mat-icon> דו"ח</a>
    </mat-toolbar>
    <router-outlet />
  `,
  styles: [`
    .spacer { flex: 1 1 auto; }
    .brand { display:flex; align-items:center; gap:8px; font-weight: 600; }
    a.active { text-decoration: underline; }
  `]
})
export class AppComponent {}
