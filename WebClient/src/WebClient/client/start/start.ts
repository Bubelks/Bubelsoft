///<amd-module name="start/start"/>

import { Register } from "start/register/register";
import { Login } from "start/login/login";

export class Start {
    public register: Register;
    public login: Login;

    constructor() {
        this.register = new Register();
        this.login = new Login();
    }
}