import {Pipe, PipeTransform} from "@angular/core";
import {TopicModel} from "../../models/topic/topic.model";

@Pipe({
  name: 'topicsFilter'
})
export class TopicsFilterPipe implements PipeTransform {
  transform(topics: TopicModel[], search: string = ''): TopicModel[] {
    if (!search.trim()) {
      return topics;
    }

    return topics.filter(t => {
      return t?.title?.toLowerCase().indexOf(search.toLowerCase()) !== -1;
    })
  }
}
