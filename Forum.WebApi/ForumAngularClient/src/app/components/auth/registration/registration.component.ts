import { Component, OnInit } from '@angular/core';
import {RegistrationModel} from "../../../models/user/registration.model";
import {UserService} from "../../../services/user.service";

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  model: RegistrationModel = new RegistrationModel();
  constructor(private as: UserService) { }

  ngOnInit(): void {
  }

  register() {
    this.as.registration(this.model);
  }
}
