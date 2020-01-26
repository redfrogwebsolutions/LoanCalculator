import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoanDetails } from '../model/loan-details';
import { LoanSummary } from '../model/loan-summary';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
}

@Injectable({
  providedIn: 'root'
})
export class LoanCalculatorService {    

  private _baseUrl: string;
  
  constructor(private http: HttpClient) {
    this._baseUrl = "https://localhost:5001/";    
   }

   public async calculateNoFeeLoan(loanSummary: LoanSummary): Promise<LoanDetails>{
       return await this.http.post<LoanDetails>(this._baseUrl + 'api/LoanNoFeeCalculator', JSON.stringify(loanSummary), httpOptions).toPromise();
   }
}