#lang racket


;   x ->->->
; y 0,0  1,0
;
; | 0,1  1,1
; v
; | 0,2  1,2
; v

(define DEV #f)
(define input-path (make-parameter "04.txt"))
(when DEV (input-path "04-sample.txt"))

; build a mapping from coordinate to letter
(define (build-search-mapping path)
  (for/fold ([search (hash)]
             [x 0]
             #:result search)
            ([line (file->lines path)])
    (for/fold ([search search]
               [y 0]
               #:result (values search (add1 x)))
              ([char (string->list line)])
      (values
       (hash-set search (list x y) char)
       (add1 y)))))

; Build directions and move from one to another
(define dirs '(N S E W NE NW SW SE))
(define (step from dir)
  (match-let ([(list x y) from])
    (match dir
      ['N  (list       x  (sub1 y))]
      ['S  (list       x  (add1 y))]
      ['E  (list (add1 x)       y)]
      ['W  (list (sub1 x)       y)]
      ['NW (list (sub1 x) (sub1 y))]
      ['NE (list (add1 x) (sub1 y))]
      ['SW (list (sub1 x) (add1 y))]
      ['SE (list (add1 x) (add1 y))])))

; Search for word at a location in a direction
(define (has-word? word-search at word dir)
  (cond
    [(equal? word "") #t]
    [(equal? (hash-ref word-search at #f) (string-ref word 0))
     (has-word? word-search (step at dir) (substring word 1) dir)]
    [else #f]))

; Count all occurrences of a word at a location in all directions
(define (count-occurrences-at word-search index word [dir #f])
  (count identity (map (curry has-word? word-search index word) dirs)))

; Count all occurrences of the word in a word search
(define (count-occurrences word-search word)
  (for/fold ([count 0])
            ([index (hash-keys word-search)])
    (+ count (count-occurrences-at word-search index word))))

; Read data -------------------------------------------------

(define word-search (build-search-mapping (input-path)))
    
; Part 1 ----------------------------------------------------

(count-occurrences word-search "XMAS")







