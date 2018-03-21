namespace SampleApp

open System

type Post =
  { id : int
  ; author : string
  ; title : string
  ; text : string
  ; created_at : DateTime
  }