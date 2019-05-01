///<amd-module name="start/start"/>

import { Register } from "start/register/register";

export class Start {
    public register: Register;

    constructor() {
        this.register = new Register();
    }
}