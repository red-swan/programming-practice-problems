#lang racket


; Functions -------------------------------------------------------------

(define (string->instruction s)
  (match-let ([(list _ dir amount) (regexp-match #px"^(L|R)(\\d+)" s)])
    (list
     (if (equal? dir "L") 'L 'R)
     (string->number amount))))
      

(define (turn instruction from [normalize #t])
  (match-let ([(list dir amount) instruction])
    
    [(list 'L n) (- from n)]
    [(list 'R n) (+ from n)]
    [_ (error "Invalid instruction")]))
    
    

; Reading Data ----------------------------------------------------------
(define instructions (map string->instruction (file->lines "sample.txt")))

(foldl turn 50 instructions)


(define codes
  (for/fold ([acc '(50)]
             [x 50]
             #:result (reverse acc))
            ([instruction instructions])
    (let ([x* (turn instruction x)])
      (values (cons x* acc) x*))))

; Part 1 ----------------------------------------------------------------
; (count zero? codes)
