#lang racket



; Environment setup -------------------------------------------------------------
(require racket/hash)

(define DEV #f)
(define input-path (make-parameter "04.txt"))
(when DEV (input-path "04-sample.txt"))


; Hash Functions ----------------------------------------------------------------

(define (hash-equal-at? ht1 ht2 key)
  (and
   (hash-has-key? ht1 key)
   (hash-has-key? ht2 key)
   (equal? (hash-ref ht1 key) (hash-ref ht2 key))))


; Coordinate Functions ----------------------------------------------------------

;Add x y coordinates stored as lists
(define (add pt1 pt2)
  (match-let ([(list x1 y1) pt1]
              [(list x2 y2) pt2])
    (list (+ x1 x2) (+ y1 y2))))


; Build directions and move from one to another
(define directions '(N S E W NE NW SW SE))

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

(define (turn-around dir)
  (match dir
    ['N 'S]
    ['S 'N]
    ['E 'W]
    ['W 'E]
    ['NW 'SE]
    ['SW 'NE]
    ['SE 'NW]
    ['NE 'SW]))


; Word Search Functions ---------------------------------------------------------
; read a file and build a mapping from coordinate to letter
(define (build-search-mapping path)
  (for/fold ([search (hash)]
             [y 0]
             #:result search)
            ([line (file->lines path)])
    (for/fold ([search search]
               [x 0]
               #:result (values search (add1 y)))
              ([char (string->list line)])
      (values
       (hash-set search (list x y) char)
       (add1 x)))))

; Build patterns for words in a direction
(define (build-word-pattern dir word [start (list 0 0)])
  (for/fold ([at start]
             [indices (hash)]
             #:result indices)
            ([char (string->list word)])
    (values
     (step at dir)
     (hash-set indices at char))))

(define (build-word-patterns word)
  (map (curryr build-word-pattern word) directions))


; Searching for patterns in a word search

(define (has-pattern-at? word-search at pattern)
  (for/fold ([errored? #f]
             #:result (not errored?))
            ([coordinate (hash-keys pattern)])
      (if errored?
          errored?
          (let ([coordinate* (add at coordinate)])
            (not
             (and
              (hash-has-key? word-search coordinate*)
              (equal?
               (hash-ref word-search coordinate*)
               (hash-ref pattern coordinate))))))))

(define (count-patterns-at word-search at patterns)
  (count (curry has-pattern-at? word-search at) patterns))

(define (count-occurrences word-search counter)
  (for/fold ([occurrences 0])
            ([coordinate (hash-keys word-search)])
    (let* ([letter (hash-ref word-search coordinate)]
           [count (counter word-search coordinate letter)])
      (+ occurrences count))))

(define (build-simple-counter trigger-letter patterns)
  (λ (ws coord word-search-letter)
      (if (equal? word-search-letter trigger-letter)
          (count-patterns-at ws coord patterns)
          0)))

; Computing Solutions ---------------------------------------

; Read data -----------------------------
(define word-search (build-search-mapping (input-path)))

; Part 1 --------------------------------
(define xmas-patterns (build-word-patterns "XMAS"))

(time
 (let ([count-xmas-patterns (build-simple-counter #\X xmas-patterns)])
   (count-occurrences word-search count-xmas-patterns))
 )
; Part 2 --------------------------------

(define cross-mass-patterns
  (let* ([dirs-of-interest  '((SE SW) (SW NW) (NW NE) (NE SE))]
         [build-word-from (λ (dir) (build-word-pattern (turn-around dir) "MAS" (step (list 0 0) dir)))]
         [build-pattern-from (λ (dirs) (apply hash-union (map build-word-from dirs) #:combine (λ (a b) a)))])
    (map build-pattern-from dirs-of-interest)))

(time
 (let ([count-cross-mass-patterns (build-simple-counter #\A cross-mass-patterns)])
   (count-occurrences word-search count-cross-mass-patterns))
 )
