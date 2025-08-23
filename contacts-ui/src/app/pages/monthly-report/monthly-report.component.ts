import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../services/api.service';
import { MonthlyReportRow } from '../../models/contact';

@Component({
  selector: 'app-monthly-report',
  standalone: true,
  imports: [
    CommonModule, FormsModule,MatIconModule,
    MatCardModule, MatTableModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatProgressBarModule, MatIconModule
  ],
  templateUrl: './monthly-report.component.html',
  styleUrls: ['./monthly-report.component.scss']
})
export class MonthlyReportComponent {
  private api = inject(ApiService);

  year = new Date().getFullYear();
  rows: MonthlyReportRow[] = [];
  loading = false;

  displayedColumns: string[] = ['month', 'department', 'count'];

  load(): void {
    this.loading = true;
    this.api.getMonthlyReport(this.year).subscribe({
      next: (data: MonthlyReportRow[]) => { this.rows = data; this.loading = false; },
      error: (err: unknown) => { console.error(err); this.loading = false; }
    });
  }

  ngOnInit(): void { this.load(); }
}
