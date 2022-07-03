import {Injectable} from "@angular/core";
import {TopicService} from "./topic.service";
import {TopicModel} from "../models/topic/topic.model";

@Injectable({
  providedIn : 'root'
})
export class TopicHelperService {
  constructor(private ts: TopicService) {
  }

  increase(topic: any, topics: TopicModel[] | undefined) {
    this.ts.increaseRating(topic.id)
      .subscribe(() => {
        if (topics !== undefined) {
          let index = topics!.findIndex(t => t.id == topic.id);
          // @ts-ignore
          topics[index].rating++;
        } else {
          topic.rating++;
        }
      });
  }

  increaseRating(topic: any, topics: TopicModel[] | undefined) {
    if (topic.isRatedDown && !topic.isRatedUp) {
      this.increase(topic, topics);
      this.increase(topic, topics);
      topic.isRatedDown = false;
      topic.isRatedUp = true;

      return;
    }
    if (!topic.isRatedUp || topic.isRatedDown) {
      this.increase(topic, topics);
      topic.isRatedUp = true;
      topic.isRatedDown = false;
      return;
    }

    this.decrease(topic, topics);
    topic.isRatedUp = false;
  }

  decreaseRating(topic: any, topics: TopicModel[] | undefined) {
    if (!topic.isRatedDown && topic.isRatedUp) {
      this.decrease(topic, topics);
      this.decrease(topic, topics);
      topic.isRatedDown = true;
      topic.isRatedUp = false;

      return;
    }

    if (!topic.isRatedDown) {
      this.decrease(topic, topics);
      topic.isRatedDown = true;
      return;
    }

    this.increase(topic, topics);
    topic.isRatedDown = false;
  }

  decrease(topic: any, topics: TopicModel[] | undefined) {
    this.ts.decreaseRating(topic.id)
      .subscribe(() => {
        if (topics !== undefined) {
          let index = topics!.findIndex(t => t.id == topic.id);
          // @ts-ignore
          topics[index].rating--;
        } else {
          topic.rating--;
        }
      });
  }

  sortTop(isSorted: boolean, posts: TopicModel[] | undefined, postsCopy: TopicModel[], isMainPage = false, isUserPage = false) {
    if (!isSorted) {
      if (!isMainPage || !isUserPage) {
        let pinned = posts!.filter(t => t.isPinned);
        let notPinned = posts!.slice(pinned!.length);
        postsCopy = pinned.concat(notPinned);
        let notPinnedCopy = [...notPinned];
        // @ts-ignore
        notPinnedCopy.sort((a, b) => b.rating - a.rating);

        posts = pinned.concat(notPinnedCopy);
        isSorted = true;
        return {posts, isSorted, postsCopy};
      }
      // @ts-ignore
      posts.sort((a, b) => b.rating - a.rating);
      isSorted = true;
      return {posts, isSorted, postsCopy};
    }

    posts = [...postsCopy];
    isSorted = false;

    return {posts, isSorted, postsCopy};
  }

  sortNew(isSorted: boolean, posts: TopicModel[] | undefined, postsCopy: TopicModel[], isMainPage = false, isUserPage = false) {
    if (!isSorted) {
      if (!isMainPage || !isUserPage) {
        let pinned = posts!.filter(t => t.isPinned);
        let notPinned = posts!.slice(pinned!.length);
        postsCopy = pinned.concat(notPinned);
        let notPinnedCopy = [...notPinned];

        notPinnedCopy.sort((a, b) => {
          // @ts-ignore
          return b.postingDate.localeCompare(a.postingDate);
        });

        posts = pinned.concat(notPinnedCopy);
        isSorted = true;
        return {posts, isSorted, postsCopy};
      }

      // @ts-ignore
      posts.sort((a, b) => {
        // @ts-ignore
        return b.postingDate.localeCompare(a.postingDate);
      });
      isSorted = true;
      return {posts, isSorted, postsCopy};
    }

    posts = [...postsCopy];
    isSorted = false;
    return {posts, isSorted, postsCopy};
  }
}
