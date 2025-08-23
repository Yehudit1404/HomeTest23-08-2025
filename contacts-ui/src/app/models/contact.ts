export interface ContactRequest {
  id?: string; // אם בשרת עברתם ל-int: number
  name: string;
  phone: string;
  email: string;
  departments: string[];
  description: string;
  createdAtUtc?: string;
}

export interface MonthlyReportRow {
  year: number;
  month: number;
  department: string;
  count: number;
}
