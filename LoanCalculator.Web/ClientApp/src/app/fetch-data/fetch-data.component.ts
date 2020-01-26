import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoanDetails } from '../model/loan-details';

@Component({
    selector: 'app-fetch-data',
    templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
    @Input() loanForDisplay: LoanDetails;

    constructor() {

    }
}


