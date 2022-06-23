import { Component, OnInit } from '@angular/core';
import {LoginModel} from "../../../models/user/login.model";
import {UserService} from "../../../services/user.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: LoginModel = new LoginModel();
  constructor(private as: UserService) { }

  ngOnInit(): void {
  }

  login() {
    this.as.login(this.model);
  }
}
