using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class convert{
  Fold edges_vertices_to_vertices_vertices_unsorted(Fold fold){
    fold.vertices_vertices = filter.edges_vertices_to_vertices_vertices(fold);
    return fold;
  }
  Fold edges_vertices_to_vertices_vertices_sorted(Fold fold){
    fold.vertices_vertices = filter.edges_vertices_to_vertices_vertices(fold);
    sort_vertices_vertices(fold);
  }

  Fold sort_vertices_vertices(Fold fold){
    for(int i = 0; i<fold.vertices_vertices.Count; i++){
      // filter.sortbyangle
    }
  }
}
