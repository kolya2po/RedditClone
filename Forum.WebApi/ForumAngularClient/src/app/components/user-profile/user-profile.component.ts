import {Component, Input } from '@angular/core';
import {UserModel} from "../../models/user/user.model";
import {UserService} from "../../services/user.service";
import {UpdateUserModel} from "../../models/user/update-user.model";
import {catchError, tap} from "rxjs";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent {
  @Input() user: UserModel = new UserModel();
  updateUserModel = new UpdateUserModel();
  isEditing = false;

  constructor(public us: UserService,
              private _snackBar: MatSnackBar) {
  }

  ngOnChanges() {
    this.updateUserModel.id = this.user.id;
    // @ts-ignore
    this.updateUserModel.userName = this.user.userName;
    // @ts-ignore
    this.updateUserModel.birthDate =  this.user.birthDate;
  }

  updateUser() {
    this.us.update(this.updateUserModel)
      .pipe(tap(() => {
      }), catchError((err) => {
        this.openSnackBar('You must be at least 7 years old.');
        throw err;
      }))
      .subscribe(() => {
        this.us.getById(this.user.id)
          .subscribe(u => {
            this.us.user = u;
            this.user.userName = u.userName;
            this.user.birthDate = u.birthDate;
          });

        this.isEditing = false;
      });
  }

  openSnackBar(message: any) {
    this._snackBar.open(message, '', {
      duration: 2000
    });
  }
}
