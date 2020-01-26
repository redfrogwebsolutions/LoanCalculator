import { LoanSummary } from "./loan-summary";
import { PaymentScheduleItem } from "./payment-schedule-item";

export interface LoanDetails {
    summary: LoanSummary;
    payments: PaymentScheduleItem[];
}
