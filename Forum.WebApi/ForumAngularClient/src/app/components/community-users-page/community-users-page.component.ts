import { Component, OnInit } from '@angular/core';
import {CommunityService} from "../../services/community.service";
import {UserModel} from "../../models/user/user.model";
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-community-users-page',
  templateUrl: './community-users-page.component.html',
  styleUrls: ['./community-users-page.component.css']
})
export class CommunityUsersPageComponent implements OnInit {
  users: UserModel[] = [];
  search = '';
  name = '';
  id: any;
  sub:any;
  constructor(private cs: CommunityService,
              private route: ActivatedRoute,
              public us: UserService) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(p =>{
      this.name = p['name'];
      this.id = +p['id'];
      this.cs.getAllUsers(this.id)
        .subscribe(u => {
          this.users = u;
        });
    })
  }

  deleteUserFromCommunity(userId: any) {
    this.cs.leave(userId, this.id)
      .subscribe(() => {
        this.users = this.users.filter(u => u.id !== userId);
      });
  }

  setModerator(id: any, user: UserModel) {
    this.cs.addModerator(id, this.id)
      .subscribe(() => user.moderatedCommunityId = this.id);
  }

  removeModerator(id: any, user: UserModel) {
    this.cs.removeModerator(id, this.id)
      .subscribe(() => user.moderatedCommunityId = undefined);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
