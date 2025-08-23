import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContactRequest, MonthlyReportRow } from '../models/contact';
import { API_BASE_URL } from '../app.config';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private base = API_BASE_URL;

  getContacts(): Observable<ContactRequest[]> {
    return this.http.get<ContactRequest[]>(`${this.base}/api/contacts`);
  }
  getContact(id: string | number): Observable<ContactRequest> {
    return this.http.get<ContactRequest>(`${this.base}/api/contacts/${id}`);
  }
  createContact(payload: ContactRequest): Observable<ContactRequest> {
    return this.http.post<ContactRequest>(`${this.base}/api/contacts`, payload);
  }
  updateContact(id: string | number, payload: ContactRequest): Observable<ContactRequest> {
    return this.http.put<ContactRequest>(`${this.base}/api/contacts/${id}`, payload);
  }
  deleteContact(id: string | number) {
    return this.http.delete(`${this.base}/api/contacts/${id}`);
  }
  getMonthlyReport(year: number): Observable<MonthlyReportRow[]> {
    return this.http.get<MonthlyReportRow[]>(`${this.base}/api/reports/monthly/${year}`);
  }
}
