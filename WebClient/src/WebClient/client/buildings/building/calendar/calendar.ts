///<amd-module name="buildings/building/calendar/calendar"/>

import { Report } from "buildings/building/report/report"
import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class Calendar {
    private buildingId: number;
    private yearSub: ko.subscription<number>;
    private monthSub: ko.subscription<ISelectValue>;

    public avaiableYears = [2017, 2018, 2019];
    public avaiableMonths: ISelectValue[] = [
        { value: 1, displayValue: "January" },
        { value: 2, displayValue: "February" },
        { value: 3, displayValue: "March" },
        { value: 4, displayValue: "April" },
        { value: 5, displayValue: "May" },
        { value: 6, displayValue: "June" },
        { value: 7, displayValue: "July" },
        { value: 8, displayValue: "August" },
        { value: 9, displayValue: "September" },
        { value: 10, displayValue: "October" },
        { value: 11, displayValue: "November" },
        { value: 12, displayValue: "December" }
    ];

    public year: ko.Observable<number>;
    public month: ko.Observable<ISelectValue>;

    public reports: ko.ObservableArray<IReports>;
    public reportDetails: ko.Observable<Report>;
    public readonly modalOpen: ko.Observable<boolean>;

    constructor(buildingId: number) {
        this.buildingId = buildingId;

        this.reportDetails = ko.observable(null);
        this.modalOpen = ko.observable(false);

        var currentDate = new Date(Date.now());
        this.year = ko.observable(currentDate.getFullYear());
        const month = this.avaiableMonths.filter(m => m.value === currentDate.getMonth() + 1)[0];
        this.month = ko.observable(month);
        this.reports = ko.observableArray([]);

        this.yearSub = this.year.subscribe(() => this.refresh());
        this.monthSub = this.month.subscribe(() => this.refresh());
        this.refresh();
    }

    public openModal = (report: IReport) => {
        this.reportDetails(new Report(this.buildingId, report.id));

        this.modalOpen(true);
        $("body").addClass("modal-open");
        const modal = $("#reportDetails");
        modal.addClass("show");
        modal.css("display", "block");
    }

    public closeModal = () => {
        this.reportDetails(null);

        this.modalOpen(false);
        $("body").removeClass("modal-open");
        var modal = $("#reportDetails");
        modal.removeClass("show");
        modal.css("display", "none");
    }

    private refresh = () => {
        this.getReports()
            .done((data: IReports[]) =>
                this.reports(data.map(d => ({
                    day: d.day,
                    reports: d.reports,
                    openModal: this.openModal
                })))
            );
    }

    public saveReport(): void {
        const dto = this.reportDetails().getDto();
        rest.post("building", `${this.buildingId}/report/${this.reportDetails().reportId}`, dto);
    }

    private getReports(): JQueryPromise<IReports[]> {
        return rest.post("building", `${this.buildingId}/reports`, { month: this.month().value, year: this.year() });
    }
}

interface IReports {
    day: number;
    reports: IReport[];
    openModal: (report: IReport) => void;
}

interface IReport {
    id: number;
    companyName: string;
    userName: string;
}