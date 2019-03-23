///<amd-module name="buildings/building/building"/>

import { Companies, IBuildingCompany } from "buildings/building/companies/companies";
import { Calendar } from "buildings/building/calendar/calendar";
import { Estimation } from "buildings/building/estimation/estimation";
import { Report } from "buildings/building/report/report";
import { Reports } from "buildings/building/reports/reports";

import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class Building {
    private id: number;
    public companies: ko.Observable<Companies>;
    public calendar: ko.Observable<Calendar>;
    public estimation: ko.Observable<Estimation>;
    public newReport: ko.Observable<Report>;
    public reports: ko.Observable<Reports>;
    public useCalendar: ko.Observable<boolean>;
    public useEstimation: ko.Observable<boolean>;
    public useReports: ko.Observable<boolean>;
    public buildingName: ko.Observable<string>;
    public canReport: ko.Observable<boolean>;

    constructor(buildingId: number) {
        this.id = buildingId;
        this.useCalendar = ko.observable(true);
        this.useEstimation = ko.observable(false);
        this.calendar = ko.observable(new Calendar(this.id));
        this.estimation = ko.observable(null);
        this.companies = ko.observable(null);
        this.buildingName = ko.observable("");
        this.canReport = ko.observable(false);
        this.useReports = ko.observable(false);
        this.newReport = ko.observable(null);
        this.reports = ko.observable(null);

        this.getBuilding(buildingId)
            .done((data: IBuilding) => {
                this.buildingName(data.name);
                this.canReport(true);
            });

        this.getCompanies(buildingId)
            .done((data: IBuildingCompany[]) => {
            this.companies(new Companies(data, this.id));
            this.estimation(new Estimation(buildingId, data));
            this.reports(new Reports(buildingId, data));
        });
    }

    public openAddReport(): void {
        this.newReport(new Report(this.id));
    }

    public addReport(): void {
        const dto = this.newReport().getDto();
        rest.put("buildings", `${this.id}/report`, dto);
    }

    public toggleCard = (clicked: string, _, event) => {
        if (clicked === "calendar" && this.useCalendar()) return;
        if (clicked === "estimation" && this.useEstimation()) return;
        if (clicked === "reports" && this.useReports()) return;

        $(event.currentTarget).blur();
        this.useCalendar(false);
        this.useEstimation(false);
        this.useReports(false);

        switch (clicked) {
            case "calendar":
                this.useCalendar(true);
                break;
            case "estimation":
                this.useEstimation(true);
                break;
            case "reports":
                this.useReports(true);
                break;
        }
    }

    private getCompanies(buildingId: number): JQueryPromise<IBuildingCompany[]> {
        return rest.get("buildings", `${buildingId}/companies`);
    }

    private getBuilding(buildingId: number): JQueryPromise<IBuilding> {
        return rest.get("buildings", `${buildingId}`);
    }

    public dispose(): void {
        
    }
}

interface IBuilding {
    name: string;
    canReport: boolean;
}