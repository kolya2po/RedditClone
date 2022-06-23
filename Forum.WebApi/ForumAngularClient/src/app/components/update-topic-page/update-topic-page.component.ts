import { Component, OnInit } from '@angular/core';
import {TopicService} from "../../services/topic.service";
import {ActivatedRoute} from "@angular/router";
import {RouterExtService} from "../../routing/routerExt.service";
import {UpdateTopicModel} from "../../models/topic/update-topic.model";
import {TopicModel} from "../../models/topic/topic.model";

@Component({
  selector: 'app-update-topic-page',
  templateUrl: './update-topic-page.component.html',
  styleUrls: ['./update-topic-page.component.css']
})
export class UpdateTopicPageComponent implements OnInit {
  sub: any;
  updateTopicModel = new UpdateTopicModel();
  topic = new TopicModel();

  constructor(private ts: TopicService,
              private route: ActivatedRoute,
              private routeExt: RouterExtService) { }

  ngOnInit(): void {
    this.sub = this.route.params.subscribe(p => {
      let id = +p['id'];

      this.ts.getById(id)
        .subscribe(t => {
          this.topic = t;
          this.updateTopicModel.id = this.topic.id;
          this.updateTopicModel.postingDate = this.topic.postingDate;
          this.updateTopicModel.authorId = this.topic.authorId;
          this.updateTopicModel.communityId = this.topic.communityId;
          this.updateTopicModel.rating = this.topic.rating;
          this.updateTopicModel.title = this.topic.title;
          this.updateTopicModel.text = this.topic.text;
        });
    });
  }


  updateTopic() {
    this.ts.update(this.updateTopicModel)
      .subscribe(() => {
        this.goToPreviousPage();
      });
  }

  goToPreviousPage() {
    let prev = this.routeExt.getPreviousUrl();
    this.routeExt.router.navigate([prev]);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
