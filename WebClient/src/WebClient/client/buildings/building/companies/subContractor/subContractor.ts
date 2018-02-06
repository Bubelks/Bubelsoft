///<amd-module name="buildings/building/companies/subContractor/subContractor"/>

import { CompanyStep } from "buildings/create/steps/company";

import * as ko from "knockout";
import * as rest from "utils/communication/rest";
import * as navigator from "utils/navigator";

export class SubContractor {
    private buildingId: number;

    public readonly company: ko.Observable<CompanyStep>;
    public readonly modalOpen: ko.Observable<boolean>;
    constructor(buildingId: number) {
        this.buildingId = buildingId;
        this.company = ko.observable(new CompanyStep());
        this.modalOpen = ko.observable(false);
        this.openModal();
    }

    public saveCompany(): void {
        var dto = this.company().getDto();
        rest.post("company",
            "",
            {
                name: dto.name,
                nip: dto.nip,
                phoneNumber: dto.phoneNumber,
                email: dto.email,
                city: dto.city,
                postCode: dto.postCode,
                street: dto.street,
                placeNumber: dto.placeNumber,
                buildingId: this.buildingId
            }).done(data => {
                this.closeModal();
                navigator.navigate(`user/register/${data}`);
        });
    }

    private closeModal(): void {
        this.modalOpen(false);
        $("body").removeClass("modal-open");
        var modal = $("#addSubContractor");
        modal.removeClass("show");
        modal.css("display", "none");
    }

    private openModal(): void {
        this.modalOpen(true);
        $("body").addClass("modal-open");
    }

    public dispose(): void {
        
    }
}