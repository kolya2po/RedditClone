import { Component, OnInit } from '@angular/core';
import {CommunityModel} from "../../models/community/community.model";
import {TopicService} from "../../services/topic.service";
import {CommunityService} from "../../services/community.service";
import {UserService} from "../../services/user.service";
import {CreateTopicModel} from "../../models/topic/create-topic.model";
import {RouterExtService} from "../../routing/routerExt.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-create-topic-page',
  templateUrl: './create-topic-page.component.html',
  styleUrls: ['./create-topic-page.component.css']
})
export class CreateTopicPageComponent implements OnInit {
  communities: CommunityModel[] = [];
  createTopicModel = new CreateTopicModel();
  sub: any;

  constructor(private ts: TopicService,
              private cs: CommunityService,
              private us: UserService,
              private routeExt: RouterExtService,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.cs.getAll()
      .subscribe(c => this.communities = c);

    this.sub = this.route.params.subscribe(p => {
        if (p['id']) {
          this.createTopicModel.communityId = +p['id'];
        }
    });
  }

  createTopic() {
    this.createTopicModel.authorId = this.us.user.id;

    this.ts.add(this.createTopicModel)
      .subscribe(() => {
        this.goToPreviousPage();
      })
  }

  goToPreviousPage() {
    let prev = this.routeExt.getPreviousUrl();
    this.routeExt.router.navigate([prev]);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
