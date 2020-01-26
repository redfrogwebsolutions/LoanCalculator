import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, AbstractControl, ValidatorFn, FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import { LoanDetails } from '../model/loan-details';
import { LoanCalculatorService } from '../services/loan-salculator.service';
import { LoanSummary } from '../model/loan-summary';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
})

export class HomeComponent {

    loanSummaryForm: FormGroup;
    loanDetails: LoanDetails;



    constructor(private fb: FormBuilder, private loanService: LoanCalculatorService) {
        this.initForm();
    }
    loanLenghts = [{
        value: '12', description: '1 year'
    },
    {
        value: '24', description: '2 years'
    },
    {
        value: '36', description: '3 years'
    }]

    initForm() {
        this.loanSummaryForm = this.fb.group({
            fullPrice: new FormControl('', Validators.required),
            deposit: new FormControl('', Validators.required),
            deliveryDate: new FormControl('', Validators.required),
            loanLenght: new FormControl('', Validators.required)
        },
            {
                validator: this.minDepositValidator
            });
    }

    minDepositValidator(form: FormGroup) {

        const condition = (form.get('deposit').value < (form.get('fullPrice').value * 0.15)) && form.get('deposit').dirty;
        return condition ? { depositToLow: true } : null;

    }
    async calculateLoan() {
        if (!this.loanSummaryForm.invalid) {
            let summary = <LoanSummary>this.loanSummaryForm.value;

            this.loanDetails = await this.loanService.calculateNoFeeLoan(summary);

            var aa = this.loanDetails;
        }
    }
}
