import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-contact-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,MatIconModule,
    MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule,
    MatButtonModule, MatSnackBarModule, MatProgressBarModule, MatIconModule
  ],
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent {
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  private sb  = inject(MatSnackBar);

  loading = false;

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    phone: ['', [Validators.required, Validators.maxLength(30)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
    departments: [[] as string[], [Validators.required]],
    description: ['', [Validators.required, Validators.maxLength(2000)]]
  });

  availableDepartments = ['תמיכה', 'משאבי אנוש', 'IT', 'כספים', 'תפעול'];

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.sb.open('נא למלא את כל השדות הנדרשים', 'סגור', { duration: 2500 });
      return;
    }
    this.loading = true;
    this.api.createContact(this.form.value as any).subscribe({
      next: () => {
        this.loading = false;
        this.sb.open('הפנייה נשלחה בהצלחה', 'סגור', { duration: 1600 });
        this.form.reset({ departments: [] });
      },
      error: (err: unknown) => {
        console.error(err);
        this.loading = false;
        this.sb.open('שגיאה בשליחה — נסי שוב', 'סגור', { duration: 3000 });
      }
    });
  }
}
