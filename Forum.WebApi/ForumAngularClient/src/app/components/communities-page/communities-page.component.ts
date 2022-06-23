import { Component, OnInit } from '@angular/core';
import {CommunityModel} from "../../models/community/community.model";
import {CommunityService} from "../../services/community.service";
import {UserService} from "../../services/user.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-communities-page',
  templateUrl: './communities-page.component.html',
  styleUrls: ['./communities-page.component.css']
})
export class CommunitiesPageComponent implements OnInit {
  communities: CommunityModel[] = [];
  search = '';

  constructor(private cs: CommunityService,
              private us: UserService,
              private router: Router) { }

  ngOnInit(): void {
    this.cs.getAll()
      .subscribe(c => {
        this.communities = c;
      });
  }

  showJoin(id: any) {
    let userInCommunity = this.us.user.communitiesIds?.includes(id);
    return this.us.user.id === 0 || (this.us.user.id !== 0 && !userInCommunity);
  }

  join(communityId: any) {
    if (this.us.user.id === 0) {
      this.router.navigate(['login']);
      return;
    }
    this.cs.join(this.us.user.id, communityId)
      .subscribe(() => this.us.user.communitiesIds?.push(communityId));
  }

  getTopFiveCommunities() {
    let communities = [...this.communities];
    return communities.sort((a, b) => {
        // @ts-ignore
        return b.membersCount - a.membersCount;
    }).slice(0, 5);
  }
}
