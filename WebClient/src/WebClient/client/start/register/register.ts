///<amd-module name="start/register/register"/>

import { InputParameters } from "customBindings/input/input";
import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class Register {
    private currentStep: ko.Observable<RegisterStep>;

    private companyName: ko.Observable<string>;
    private companyNumber: ko.Observable<string>;

    public companyNameParams: ko.Observable<InputParameters>;
    public companyNumberParams: ko.Observable<InputParameters>;

    private userFirstName: ko.Observable<string>;
    private userLastName: ko.Observable<string>;
    private userEmail: ko.Observable<string>;
    private userPassword: ko.Observable<string>;
    private userPasswordConfirm: ko.Observable<string>;

    public userFirstNameParams: ko.Observable<InputParameters>;
    public userLastNameParams: ko.Observable<InputParameters>;
    public userEmailParams: ko.Observable<InputParameters>;
    public userPasswordParams: ko.Observable<InputParameters>;
    public userPasswordConfirmParams: ko.Observable<InputParameters>;

    public invalid: ko.Observable<boolean>;

    public modalHeader: ko.Computed<string>;
    public modalNextButton: ko.Computed<string>;
    public modalBackVisible: ko.Computed<boolean>;
    public modalFooterVisible: ko.Computed<boolean>;

    constructor() {
        this.currentStep = ko.observable(RegisterStep.Company);

        this.companyName = ko.observable("");
        this.companyNumber = ko.observable("");

        this.companyNameParams = ko.observable<InputParameters>(
        {
            value: this.companyName,
            require: true
        });
        this.companyNumberParams = ko.observable<InputParameters>(
        {
            value: this.companyNumber,
            require: true
        });

        this.userFirstName = ko.observable("");
        this.userLastName = ko.observable("");
        this.userEmail = ko.observable("");
        this.userPassword = ko.observable("");
        this.userPasswordConfirm = ko.observable("");

        this.userFirstNameParams = ko.observable<InputParameters>(
        {
            value: this.userFirstName,
            require: true
        });
        this.userLastNameParams = ko.observable<InputParameters>(
        {
            value: this.userLastName,
            require: true
        });
        this.userEmailParams = ko.observable<InputParameters>(
        {
            value: this.userEmail,
            require: true
        });
        this.userPasswordParams = ko.observable<InputParameters>(
        {
            value: this.userPassword,
            require: true,
            customValidate: (value: string) => {
                return value === this.userPasswordConfirm();
            },
            customValidationMsg: "Password values have to be the same"
        });
        this.userPasswordConfirmParams = ko.observable<InputParameters>(
        {
            value: this.userPasswordConfirm,
            require: true,
            customValidate: (value: string) => {
                return value === this.userPassword();
            }
        });

        this.invalid = ko.observable(false);

        this.modalHeader = ko.computed<string>(() => {
            switch (this.currentStep()) {
            case RegisterStep.Company:
                return "Company";
            case RegisterStep.User:
                return "User";
            case RegisterStep.Verify:
                return "Verify";
            case RegisterStep.Success:
                return "Success";
            case RegisterStep.Error:
                return "Error";
            case RegisterStep.Loading:
                return "Registering...";
            default:
                return "Register";
            }
        });

        this.modalNextButton = ko.computed<string>(() => {
            if (this.currentStep() === RegisterStep.Verify)
                return "Register";
            return "Next";
        });

        this.modalBackVisible = ko.computed<boolean>(() => {
            if (this.currentStep() === RegisterStep.Company)
                return false;
            return true;
        });

        this.modalFooterVisible = ko.computed<boolean>(() => {
            if (this.currentStep() >= RegisterStep.Success)
                return false;
            return true;
        });
    }

    public modalNext(): void {
        if (this.currentStep() === RegisterStep.Verify)
            this.register();
        else {
            if (this.validate()) {
                let step: number = this.currentStep();
                this.currentStep(++step);
                this.invalid(false);
            } else {
                this.invalid(true);
            }
        }
    }

    public modalBack(): void {
        if (this.currentStep() !== RegisterStep.Company) {
            let step: number = this.currentStep();
            this.currentStep(--step);
        }
    }

    private validate(): boolean {
        let result: boolean;
        switch (this.currentStep()) {
            case RegisterStep.Company:
                result = this.companyNameParams().validate();
                result = this.companyNumberParams().validate() && result;
                return result;
            case RegisterStep.User:
                result = this.userFirstNameParams().validate();
                result = this.userLastNameParams().validate() && result;
                result = this.userEmailParams().validate() && result;
                result = this.userPasswordParams().validate() && result;
                result = this.userPasswordConfirmParams().validate() && result;
                return result;
            case RegisterStep.Verify:
                return true;
            default:
                return false;
        }
    }

    private register(): void {
        this.currentStep(RegisterStep.Loading);

        rest.post("company",
                "register",
                {
                    company: {
                        name: this.companyName(),
                        number: this.companyNumber()
                    },
                    administrator: {
                        firstName: this.userFirstName(),
                        lastName: this.userLastName(),
                        email: this.userEmail(),
                        password: this.userPassword(),
                    }
                })
            .done(() => this.currentStep(RegisterStep.Success))
            .fail(() => this.currentStep(RegisterStep.Error));
    }
}

enum RegisterStep {
    Company = 0,
    User = 1,
    Verify = 2,
    Success = 10,
    Error = 11,
    Loading = 12,
}